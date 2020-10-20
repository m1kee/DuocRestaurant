import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { SupplyOrderReceptionPage } from './supply-order-reception.page';

describe('SupplyOrderReceptionPage', () => {
  let component: SupplyOrderReceptionPage;
  let fixture: ComponentFixture<SupplyOrderReceptionPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupplyOrderReceptionPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(SupplyOrderReceptionPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
