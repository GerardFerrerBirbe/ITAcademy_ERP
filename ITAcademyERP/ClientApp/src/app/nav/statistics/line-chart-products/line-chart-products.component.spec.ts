import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LineChartProductsComponent } from './line-chart-products.component';

describe('LineChartProductsComponent', () => {
  let component: LineChartProductsComponent;
  let fixture: ComponentFixture<LineChartProductsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LineChartProductsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LineChartProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
