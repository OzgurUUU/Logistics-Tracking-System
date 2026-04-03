import { Component, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { CourierLocation, SignalrService } from './services/signalr-service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [CommonModule]
})
export class App implements OnInit {
  private map!: L.Map;
  private markers: { [id: number]: L.Marker } = {};
  
  public activeCouriersCount = 0;
  public recentLogs: string[] = [];

  // Leaflet ikon hatasını çözmek için varsayılan ikon ayarı
  private defaultIcon = L.icon({
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});

  constructor(private signalrService: SignalrService) {}

  ngOnInit() {
    this.initMap();
    this.signalrService.startConnection();

    // SignalR'dan gelen canlı verileri dinle
    this.signalrService.locationUpdates.subscribe((data: CourierLocation) => {
      this.updateCourierOnMap(data);
    });
  }
  ngAfterViewInit() {
    // Haritaya ufak bir gecikmeyle "kendini toparla" emri veriyoruz.
    // Bu, o gri boşlukların kalıcı olarak yok olmasını sağlar.
    setTimeout(() => {
      if (this.map) {
        this.map.invalidateSize();
      }
    }, 100);
  }
  private initMap(): void {
    // Adana koordinatları ile haritayı başlat
    this.map = L.map('map', {
      center: [37.0000, 35.3200],
      zoom: 14
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    L.control.zoom({
      position: 'bottomright'
    }).addTo(this.map);
  }

  private updateCourierOnMap(data: CourierLocation): void {
    const { id, lat, lon } = data;

    if (this.markers[id]) {
      // Kurye zaten haritada varsa konumunu kaydır
      this.markers[id].setLatLng([lat, lon]);
    } else {
      // Yeni kurye geldiyse haritaya ekle
      const newMarker = L.marker([lat, lon], { icon: this.defaultIcon }).addTo(this.map);
      newMarker.bindPopup(`<b>Kurye ID:</b> ${id}`).openPopup();
      this.markers[id] = newMarker;
      this.activeCouriersCount++;
    }

    // Sol panel için log oluştur (en fazla 5 log tut)
    this.recentLogs.unshift(`Kurye ${id} hareket etti: ${lat.toFixed(4)}, ${lon.toFixed(4)}`);
    if (this.recentLogs.length > 5) this.recentLogs.pop();
  }
}