import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HhrrComponent } from './hhrr.component';

describe('HhrrComponent', () => {
  let component: HhrrComponent;
  let fixture: ComponentFixture<HhrrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HhrrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HhrrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
