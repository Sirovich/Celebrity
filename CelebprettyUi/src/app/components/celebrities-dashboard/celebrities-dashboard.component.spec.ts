import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CelebritiesDashboardComponent } from './celebrities-dashboard.component';

describe('CelebritiesDashboardComponent', () => {
  let component: CelebritiesDashboardComponent;
  let fixture: ComponentFixture<CelebritiesDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CelebritiesDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CelebritiesDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
