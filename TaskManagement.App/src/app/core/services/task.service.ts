import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/task.model';
import { CreateTask } from '../models/task-create.model';
import { UpdateTask } from '../models/task-update.model';
import { environment } from '../../../environments/environment';
import { AppConstants } from '../constants/app.constants';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl = `${environment.apiUrl}/${AppConstants.API_ENDPOINTS.TASKS}`;

  constructor(private http: HttpClient) {}

  getTasks(userId: number): Observable<Task[]> {
    return this.http.get<Task[]>(`${this.apiUrl}/${userId}`);
  }

  getTaskById(userId: number, taskId: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${userId}/${taskId}`);
  }

  createTask(userId: number, task: CreateTask): Observable<Task> {
    return this.http.post<Task>(`${this.apiUrl}/${userId}`, task);
  }

  updateTask(
    userId: number,
    taskId: number,
    task: UpdateTask
  ): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${userId}/${taskId}`, task);
  }

  deleteTask(userId: number, taskId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${userId}/${taskId}`);
  }
}
