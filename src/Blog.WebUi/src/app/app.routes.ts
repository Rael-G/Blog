import { Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { AboutComponent } from './components/pages/about/about.component';
import { ContactComponent } from './components/pages/contact/contact.component';
import { PostComponent } from './components/pages/post/post.component';
import { ManagementComponent } from './components/pages/management/management.component';
import { CreatePostComponent } from './components/pages/create-post/create-post.component';

export const routes: Routes =
    [
        { path: '', component: HomeComponent },
        { path: 'about', component: AboutComponent },
        { path: 'contact', component: ContactComponent },
        { path: 'posts/example', component: PostComponent },
        { path: 'management', component: ManagementComponent },
        { path: 'create-post', component: CreatePostComponent },
        { path: 'posts/:id', component: PostComponent }
    ];
