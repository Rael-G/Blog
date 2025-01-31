import { Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { PostComponent } from './components/pages/post/post.component';
import { ManagementComponent } from './components/pages/management/management.component';
import { CreatePostComponent } from './components/pages/create-post/create-post.component';
import { EditPostComponent } from './components/pages/edit-post/edit-post.component';
import { TagComponent } from './components/pages/tag/tag.component';
import { LoginComponent } from './components/pages/login/login.component';
import { authGuard } from './guards/auth/auth.guard';
import { SigninComponent } from './components/pages/signin/signin.component';
import { UserComponent } from './components/pages/user/user.component';
import { ProfileComponent } from './components/pages/profile/profile.component';

export const routes: Routes =
    [
        { path: '', component: HomeComponent },
        { path: 'management', component: ManagementComponent, canActivate: [authGuard] },
        { path: 'create-post', component: CreatePostComponent, canActivate: [authGuard] },
        { path: 'posts/:id', component: PostComponent },
        { path: 'edit-post/:id', component: EditPostComponent, canActivate: [authGuard] },
        { path: 'tags/:id', component: TagComponent },
        { path: 'login', component: LoginComponent},
        { path: 'signin', component: SigninComponent},
        { path: 'users/:id', component: UserComponent },
        { path: 'profile', component: ProfileComponent, canActivate: [authGuard]}
    ];
