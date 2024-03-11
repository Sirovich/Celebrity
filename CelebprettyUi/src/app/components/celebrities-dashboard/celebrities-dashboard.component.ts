import { Component } from '@angular/core';
import { CelebritiesService } from '../../services/celebrities-service';
import { Celebrity } from '../../models/celebrity';
import { NgFor } from '@angular/common';
import { CelebrityCardComponent } from '../celebrity-card/celebrity-card.component';

@Component({
  selector: 'app-celebrities-dashboard',
  standalone: true,
  imports: [NgFor, CelebrityCardComponent],
  templateUrl: './celebrities-dashboard.component.html',
  styleUrl: './celebrities-dashboard.component.css'
})
export class CelebritiesDashboardComponent {
  celebrities: Celebrity[] = [];

  constructor(private celebritiesService: CelebritiesService) {
    this.celebritiesService.getCelebrities().subscribe(value => this.celebrities = value); 
  }

  onDeleteItem(index: number) {
    this.celebrities.splice(index, 1);
  }

  reset() {
    this.celebritiesService.reset().subscribe(() => {
      this.celebritiesService.getCelebrities().subscribe(value => this.celebrities = value);
    });
  }
}
