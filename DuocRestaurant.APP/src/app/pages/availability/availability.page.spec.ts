import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AvailabilityPage } from './availability.page';

describe('AvailabilityPage', () => {
  let component: AvailabilityPage;
  let fixture: ComponentFixture<AvailabilityPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvailabilityPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AvailabilityPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
