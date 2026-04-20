import { EventEmitter, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

export interface CourierLocation {
  id: number;
  lat: number;
  lon: number;
  vehicleType: string; // Opsiyonel: Araç tipi bilgisi ekleyebilirsiniz
}

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection!: signalR.HubConnection;

  // Componentlerin bu veriye abone olabilmesi için Subject kullanıyoruz
  public locationUpdates = new Subject<CourierLocation>();
  public courierDeleted = new Subject<number>(); // Silinen kuryelerin ID'lerini yayınlamak için
  public orderDelivered = new EventEmitter<number>();
  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      // Buradaki URL senin .NET API'nin adresine göre değişebilir (Örn: https://localhost:7123/courierHub)
      .withUrl('https://your-backend-url/courierHub')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('⚡ SignalR Bağlantısı Başarılı!'))
      .catch(err => console.error('SignalR Hatası:', err));

    this.hubConnection.on('ReceiveLocation', (id: number, lat: number, lon: number, vehicleType: string) => {
      this.locationUpdates.next({ id, lat, lon, vehicleType });
    });
    this.hubConnection.on('CourierDeleted', (id: number) => {
      this.courierDeleted.next(id);
    });
    this.hubConnection.on('OrderDelivered', (orderId: number) => {
      this.orderDelivered.emit(orderId);
    });
  }

}