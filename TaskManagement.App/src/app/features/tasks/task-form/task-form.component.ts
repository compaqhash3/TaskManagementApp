import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Task } from '../../../core/models/task.model';
import { TaskService } from '../../../core/services/task.service';
import { NotificationService } from '../../../core/services/notification.service';
import { AppConstants } from '../../../core/constants/app.constants';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent {
  private userId!: number;
  @Input() task?: Task;
  @Input() mode: 'add' | 'edit' | 'view' | 'delete' = 'add';
  @Output() saved = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<void>();

  isReadOnly(): boolean {
    return this.mode === 'view' || this.mode === 'delete';
  }

  constructor(private taskService: TaskService, private notification: NotificationService,) {
    const storedUserId = localStorage.getItem(AppConstants.STORAGE_KEYS.USER_ID);
    this.userId = Number(storedUserId);
  }

  save(): void {

    if (this.task?.id) {
      this.taskService.updateTask(this.userId, this.task.id, this.task).subscribe({

        next: (response) => {
          this.notification.showSuccess('Task updated successfully');
          this.saved.emit();
        },
        error: (err) => {
          this.notification.showError('Failed to update task');
        }

      });
    } else {
      this.taskService.createTask(this.userId, this.task!).subscribe({

        next: (response) => {
          this.notification.showSuccess('Task created successfully');
          this.saved.emit();
        },
        error: (err) => {
          this.notification.showError('Failed to create task');
        }

      });
    }
  }

  newTask(): void {
    this.task = {
      id: 0,
      title: '',
      description: '',
      isCompleted: false,
      isDeleted: false
    };
  }

  confirmDelete() {
    if (!this.task) return;
    this.taskService
      .deleteTask(this.userId, this.task.id)
      .subscribe(() => {
        this.deleted.emit();
        this.notification.showSuccess('Task deleted successfully');
      });
  }
}
