import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TiposContaComponent } from './tipos-conta.component';

describe('TiposContaComponent', () => {
  let component: TiposContaComponent;
  let fixture: ComponentFixture<TiposContaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TiposContaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TiposContaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
