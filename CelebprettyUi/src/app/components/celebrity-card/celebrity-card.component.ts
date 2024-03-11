import { Component, EventEmitter, Input, Output, Pipe } from '@angular/core';
import { Celebrity } from '../../models/celebrity';
import { DatePipe, NgIf } from "@angular/common";
import { CelebritiesService } from '../../services/celebrities-service';

@Component({
  selector: 'app-celebrity-card',
  standalone: true,
  imports: [NgIf, DatePipe],
  templateUrl: './celebrity-card.component.html',
  styleUrl: './celebrity-card.component.css'
})
export class CelebrityCardComponent {
  @Input() celebrity!: Celebrity;
  @Output() deleteEvent: EventEmitter<void> = new EventEmitter<void>;

  constructor(private celebrityService: CelebritiesService) {
  }

  deleteCelebrity() {
    this.celebrityService.deleteCelebrity(this.celebrity).subscribe();
    this.deleteEvent.emit();
  }
}
