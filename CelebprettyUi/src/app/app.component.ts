import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CelebritiesDashboardComponent } from "./components/celebrities-dashboard/celebrities-dashboard.component";

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet, CelebritiesDashboardComponent]
})
export class AppComponent {
  title = 'CelebprettyUi';
}
