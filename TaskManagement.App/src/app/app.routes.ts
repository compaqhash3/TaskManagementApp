import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { TasksPageComponent } from './features/tasks/tasks-home.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { 
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },
  { 
    path: 'tasks',
    component: TasksPageComponent,
    canActivate: [authGuard],    
  },
  { path: '**', redirectTo: 'login' }
];
