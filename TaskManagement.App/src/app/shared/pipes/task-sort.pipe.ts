import { Pipe, PipeTransform } from '@angular/core';
import { Task } from '../../core/models/task.model';

export type TaskSortBy = 'title' | 'created' | 'status';

@Pipe({
  name: 'taskSort',
  standalone: true
})
export class TaskSortPipe implements PipeTransform {

  transform(tasks: Task[], sortBy: TaskSortBy): Task[] {
    if (!tasks || !sortBy) return tasks;

    return [...tasks].sort((a, b) => {
      switch (sortBy) {
        case 'title':
          return a.title.localeCompare(b.title);

        case 'status':
          return Number(a.isCompleted) - Number(b.isCompleted);

        case 'created':
        default:
          return b.id - a.id;
      }
    });
  }
}
