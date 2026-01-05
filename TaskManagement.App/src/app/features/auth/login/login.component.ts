import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AppConstants } from '../../../core/constants/app.constants';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username = '';
  password = '';
  private userId!: number;

  constructor(
    private authService: AuthService,
    private router: Router,
    private notification: NotificationService
  ) { }

  login(): void {

    if (!this.username || !this.password) {
      this.notification.showWarning('Please enter username and password');
      return;
    }

    this.authService.login(this.username, this.password).subscribe({

      next: () => {
        
        const storedUserName = localStorage.getItem(AppConstants.STORAGE_KEYS.USER_NAME);
        this.notification.showSuccess(`Loggin success as ${storedUserName}`);
        this.router.navigate(['/tasks'])
      },
      error: (err) => {
        this.notification.showError('Invalid credentials');
      }
    });
  }
}
