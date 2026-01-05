import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskListComponent } from './task-list/task-list.component';
import { TaskFormComponent } from './task-form/task-form.component';
import { Task } from '../../core/models/task.model';
import { TaskService } from '../../core/services/task.service';
import { AppConstants } from '../../core/constants/app.constants';
import { NotificationService } from '../../core/services/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [CommonModule, TaskListComponent, TaskFormComponent],
  templateUrl: './tasks-home.component.html',
  styleUrls: ['./tasks-home.component.scss']
})
export class TasksPageComponent implements OnInit {

  tasks: Task[] = [];
  selectedTask?: Task;
  private userId!: number;
  username!: string;
  mode: 'add' | 'edit' | 'view' | 'delete' = 'add';

  constructor(private taskService: TaskService, private notification: NotificationService, private router: Router) { }

  ngOnInit(): void {
    const storedUserId = localStorage.getItem(AppConstants.STORAGE_KEYS.USER_ID);
    const storedUserName = localStorage.getItem(AppConstants.STORAGE_KEYS.USER_NAME);
    this.username = String(storedUserName);

    if (!storedUserId) {
      this.notification.showError('Failed to update task');
      return;
    }

    this.userId = Number(storedUserId);
    this.loadTasks();
    this.createNew();
  }

  loadTasks(): void {
    this.taskService.getTasks(this.userId)
      .subscribe(tasks => this.tasks = tasks);
  }

  onSelect(task: Task): void {
    this.selectedTask = { ...task };
  }

  createNew(): void {
    this.selectedTask = {
      id: 0,
      title: '',
      description: '',
      isCompleted: false,
      isDeleted: false
    };
    this.mode = 'add'
  }

  onView(task: Task) {
    this.selectedTask = { ...task };
    this.mode = 'view';
  }

  onEdit(task: Task) {
    this.selectedTask = { ...task };
    this.mode = 'edit';
  }

  onDelete(task: Task) {
    this.selectedTask = { ...task };
    this.mode = 'delete';
  }

  refresh(): void {
    this.createNew();
    this.loadTasks();
  }

  logout(): void {
    localStorage.removeItem(AppConstants.STORAGE_KEYS.AUTH_KEY);
    localStorage.removeItem(AppConstants.STORAGE_KEYS.USER_ID);
    this.router.navigate(['/login']);
  }
}
