import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FriendAddDialogComponent } from './friend-add-dialog.component';

describe('FriendAddDialogComponent', () => {
  let component: FriendAddDialogComponent;
  let fixture: ComponentFixture<FriendAddDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FriendAddDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FriendAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
