import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Task } from '../../../core/models/task.model';
import { TaskFilterPipe } from '../../../shared/pipes/task-filter.pipe';
import { TaskSortPipe, TaskSortBy } from '../../../shared/pipes/task-sort.pipe';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [
    CommonModule,
    TaskFilterPipe,
    TaskSortPipe,
    FormsModule
  ],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent {
  @Input() tasks: Task[] = [];

  @Output() selectTask = new EventEmitter<Task>();
  @Output() resetRequested = new EventEmitter<void>();
  @Output() viewTask = new EventEmitter<Task>();
  @Output() editTask = new EventEmitter<Task>();
  @Output() deleteTask = new EventEmitter<Task>();

  searchText = '';
  statusFilter: 'all' | 'completed' | 'pending' = 'all';
  sortBy: TaskSortBy = 'created';

  selectedTask?: Task;

  // Called when a task <li> is clicked
  onSelect(task: Task): void {
    this.selectedTask = task; // for highlighting
    this.selectTask.emit(task); // notify parent component
  }

  reset(): void {
    // clear local UI state
    this.searchText = '';
    this.statusFilter = 'all';
    this.sortBy = 'created';
    this.selectedTask = undefined;

    // tell parent to reload data & clear form
    this.resetRequested.emit();
  }

  view(task: Task) {
    this.viewTask.emit(task);
  }

  edit(task: Task) {
    this.editTask.emit(task);
  }

  remove(task: Task) {
    this.deleteTask.emit(task);
  }
}
