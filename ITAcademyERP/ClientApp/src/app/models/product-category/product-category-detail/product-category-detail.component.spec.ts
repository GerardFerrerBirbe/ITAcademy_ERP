import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCategoryDetailComponent } from './product-category-detail.component';

describe('ProductCategoryDetailComponent', () => {
  let component: ProductCategoryDetailComponent;
  let fixture: ComponentFixture<ProductCategoryDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductCategoryDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductCategoryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
