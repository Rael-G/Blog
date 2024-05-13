import { Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { AboutComponent } from './components/pages/about/about.component';
import { ContactComponent } from './components/pages/contact/contact.component';
import { PostComponent } from './components/pages/post/post.component';
import { ManagementComponent } from './components/pages/management/management.component';
import { CreatePostComponent } from './components/pages/create-post/create-post.component';
import { EditPostComponent } from './components/pages/edit-post/edit-post.component';
import { TagComponent } from './components/pages/tag/tag.component';
import { LoginComponent } from './components/pages/login/login.component';
import { authGuard } from './guards/auth/auth.guard';

export const routes: Routes =
    [
        { path: '', component: HomeComponent },
        { path: 'about', component: AboutComponent },
        { path: 'contact', component: ContactComponent },
        { path: 'posts/example', component: PostComponent },
        { path: 'management', component: ManagementComponent, canActivate: [authGuard] },
        { path: 'create-post', component: CreatePostComponent, canActivate: [authGuard] },
        { path: 'posts/:id', component: PostComponent },
        { path: 'edit-post/:id', component: EditPostComponent, canActivate: [authGuard] },
        { path: 'tags/:id', component: TagComponent },
        { path: 'login', component: LoginComponent}
    ];
