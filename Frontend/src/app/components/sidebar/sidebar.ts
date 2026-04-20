import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
  standalone: true
})
export class Sidebar {
  @Input() activeCouriersCount: number = 0;
  @Input() couriersList: { id: number, lat: number, lon: number }[] = [];
  @Input() recentLogs: string[] = [];

  @Output() courierFocused = new EventEmitter<number>();


  onDragStart(event: DragEvent, type: string) {
    event.dataTransfer?.setData('drag-type', type);
  }

  triggerFocus(id: number) {
    this.courierFocused.emit(id);
  }
}
