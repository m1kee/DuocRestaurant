import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { MyPurchasesPage } from './my-purchases.page';

describe('MyPurchasesPage', () => {
  let component: MyPurchasesPage;
  let fixture: ComponentFixture<MyPurchasesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyPurchasesPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(MyPurchasesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
