import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { BookingModalPage } from './booking-modal.page';

describe('BookingModalPage', () => {
  let component: BookingModalPage;
  let fixture: ComponentFixture<BookingModalPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookingModalPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(BookingModalPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
