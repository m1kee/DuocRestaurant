import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { AssignUserModalPage } from './assign-user-modal.page';

describe('AssignUserModalPage', () => {
  let component: AssignUserModalPage;
  let fixture: ComponentFixture<AssignUserModalPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignUserModalPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(AssignUserModalPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
