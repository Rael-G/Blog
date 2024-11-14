import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostsManagementSectionComponent } from './posts-management-section.component';

describe('PostsManagementSectionComponent', () => {
  let component: PostsManagementSectionComponent;
  let fixture: ComponentFixture<PostsManagementSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostsManagementSectionComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PostsManagementSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
