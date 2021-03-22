import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InsertionOptionsComponent } from './insertion-options.component';

describe('InsertionOptionsComponent', () => {
  let component: InsertionOptionsComponent;
  let fixture: ComponentFixture<InsertionOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InsertionOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InsertionOptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
