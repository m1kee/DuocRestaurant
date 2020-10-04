import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { WithBookingPage } from './with-booking.page';

describe('WithBookingPage', () => {
  let component: WithBookingPage;
  let fixture: ComponentFixture<WithBookingPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WithBookingPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(WithBookingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
