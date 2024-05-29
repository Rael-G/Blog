import { Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { PostComponent } from './components/pages/post/post.component';
import { ManagementComponent } from './components/pages/management/management.component';
import { CreatePostComponent } from './components/pages/create-post/create-post.component';
import { EditPostComponent } from './components/pages/edit-post/edit-post.component';
import { TagComponent } from './components/pages/tag/tag.component';

export const routes: Routes =
    [
        { path: '', component: HomeComponent },
        { path: 'posts/example', component: PostComponent },
        { path: 'management', component: ManagementComponent },
        { path: 'create-post', component: CreatePostComponent },
        { path: 'posts/:id', component: PostComponent },
        { path: 'edit-post/:id', component: EditPostComponent },
        { path: 'tags/:id', component: TagComponent }
    ];
