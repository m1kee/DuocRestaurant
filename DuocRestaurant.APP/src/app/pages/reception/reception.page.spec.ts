import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ReceptionPage } from './reception.page';

describe('ReceptionPage', () => {
  let component: ReceptionPage;
  let fixture: ComponentFixture<ReceptionPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceptionPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ReceptionPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
