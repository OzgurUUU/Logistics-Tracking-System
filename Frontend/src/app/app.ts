import { Component, NgZone, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CourierLocation, SignalrService } from './services/signalr-service';

// ÇOCUK BİLEŞENLER (İsimlerin kendi klasör yapınla eşleştiğinden emin ol)
import { Sidebar } from './components/sidebar/sidebar';
import { Map } from './components/map/map';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [CommonModule, FormsModule, Sidebar, Map]
})
export class App implements OnInit {


  @ViewChild(Map) mapComponent!: Map;

  // VERİ DURUMU (STATE)
  public couriersList: { id: number, lat: number, lon: number }[] = [];
  public activeCouriersCount = 0;
  public recentLogs: string[] = [];

  // YENİ KURYE FORMU DURUMU
  public isModalOpen = false;
  public newCourierData = { name: '', vehicleType: 'Motorcycle', lastLatitude: 0, lastLongitude: 0 };
  // YENİ SİPARİŞ FORMU DURUMU
  public isOrderModalOpen = false;
  public newOrderData = { customerName: '', latitude: 0, longitude: 0 };

  constructor(private signalrService: SignalrService, private zone: NgZone) { }

  ngOnInit() {
    this.signalrService.startConnection();

    // 1. Backend'den kurye konumu geldiğinde:
    this.signalrService.locationUpdates.subscribe((data: CourierLocation) => {

      this.mapComponent.updateMarker(data.id, data.lat, data.lon, data.vehicleType,);

      this.updateLocalList(data);
    });

    // 2. Backend'den silinme emri geldiğinde:
    this.signalrService.courierDeleted.subscribe((id: number) => {

      this.mapComponent.removeCourier(id);


      this.removeFromLocalList(id);
    });
    this.signalrService.orderDelivered.subscribe((orderId: number) => {
      this.mapComponent.removeOrderMarker(orderId);
      this.recentLogs.unshift(`✅ Sipariş #${orderId} teslim edildi!`);
    });
  }

  // --- STATE YÖNETİM METODLARI ---

  private updateLocalList(data: CourierLocation): void {
    const existingCourier = this.couriersList.find(c => c.id === data.id);
    if (existingCourier) {
      existingCourier.lat = data.lat;
      existingCourier.lon = data.lon;
    } else {
      this.couriersList.push({ id: data.id, lat: data.lat, lon: data.lon });
      this.activeCouriersCount++;
    }

    // Sol panel için log oluştur (en fazla 5 log tut)
    this.recentLogs.unshift(`Kurye ${data.id} hareket etti: ${data.lat.toFixed(4)}, ${data.lon.toFixed(4)}`);
    if (this.recentLogs.length > 5) this.recentLogs.pop();
  }

  private removeFromLocalList(id: number): void {
    this.couriersList = this.couriersList.filter(c => c.id !== id);
    this.activeCouriersCount--;

    this.recentLogs.unshift(`🚨 Kurye ${id} sistemden çıkarıldı.`);
    if (this.recentLogs.length > 5) this.recentLogs.pop();
  }

  // --- ÇOCUKLARDAN (CHILD) GELEN EVENTLER ---

  // Sidebar'daki listeden kuryeye tıklandığında tetiklenir
  public focusOnCourier(id: number): void {
    const courier = this.couriersList.find(c => c.id === id);
    if (courier) {
      this.mapComponent.flyToLocation(courier.lat, courier.lon, id);
    }
  }

  // Haritaya konum bırakıldığında (Drag&Drop) MapComponent'ten tetiklenir
  public onLocationDropped(data: { lat: number, lng: number, type: string }): void {
    if (data.type === 'order') {
      this.newOrderData.latitude = data.lat;
      this.newOrderData.longitude = data.lng;
      this.isOrderModalOpen = true;
    } else {
      this.newCourierData.lastLatitude = data.lat;
      this.newCourierData.lastLongitude = data.lng;
      this.isModalOpen = true;
    }
  }
  async saveOrder() {
    try {
      const response = await fetch('https://your-backend-url/api/Order/create-and-assign', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(this.newOrderData)
      });

      if (response.ok) {
        const result = await response.json();

        this.mapComponent.addOrderMarker(result.orderId, this.newOrderData.latitude, this.newOrderData.longitude);

        this.recentLogs.unshift(`📦 Yeni Sipariş! Kurye #${result.assignedCourierId} yola çıktı.`);
        if (this.recentLogs.length > 5) this.recentLogs.pop();

        this.isOrderModalOpen = false;
        this.newOrderData = { customerName: '', latitude: 0, longitude: 0 };

      } else {
        const errorData = await response.json();
        alert(errorData.message || "Tüm kuryeler meşgul, sipariş atanamadı.");
        this.isOrderModalOpen = false;
      }
    } catch (error) {
      console.error("Sipariş verilirken hata oluştu:", error);
      alert("API bağlantı hatası!");
    }
  }
  // --- API İSTEKLERİ ---

  async saveCourier() {
    try {
      const response = await fetch('https://your-backend-url/api/Courier', { // PORTUNU KONTROL ET
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(this.newCourierData)
      });

      if (response.ok) {
        this.isModalOpen = false;
        this.newCourierData = { name: '', vehicleType: 'Motorcycle', lastLatitude: 0, lastLongitude: 0 };
      } else {
        alert("Sunucu reddetti! Backend portunu veya veritabanını kontrol et.");
      }
    } catch (error) {
      console.error("Kurye eklenirken hata oluştu:", error);
      alert("Bağlantı hatası! Backend API çalışıyor mu?");
    }
  }
}