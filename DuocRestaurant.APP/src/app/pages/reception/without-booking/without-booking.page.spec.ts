import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { WithoutBookingPage } from './without-booking.page';

describe('WithoutBookingPage', () => {
  let component: WithoutBookingPage;
  let fixture: ComponentFixture<WithoutBookingPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WithoutBookingPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(WithoutBookingPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
