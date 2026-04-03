import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

export interface CourierLocation {
  id: number;
  lat: number;
  lon: number;
}

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection!: signalR.HubConnection;
  
  // Componentlerin bu veriye abone olabilmesi için Subject kullanıyoruz
  public locationUpdates = new Subject<CourierLocation>();

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      // Buradaki URL senin .NET API'nin adresine göre değişebilir (Örn: https://localhost:7123/courierHub)
      .withUrl('https://localhost:7266/courierHub') 
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('⚡ SignalR Bağlantısı Başarılı!'))
      .catch(err => console.error('SignalR Hatası:', err));

    // Backend'deki _hubContext.Clients.All.SendAsync("ReceiveLocation", ...) metodunu dinliyoruz
    this.hubConnection.on('ReceiveLocation', (id: number, lat: number, lon: number) => {
      this.locationUpdates.next({ id, lat, lon });
    });
  }
}