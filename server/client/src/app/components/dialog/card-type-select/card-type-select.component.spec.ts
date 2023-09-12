import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardTypeSelectComponent } from './card-type-select.component';

describe('CardTypeSelectComponent', () => {
  let component: CardTypeSelectComponent;
  let fixture: ComponentFixture<CardTypeSelectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CardTypeSelectComponent]
    });
    fixture = TestBed.createComponent(CardTypeSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
