import { Pipe, PipeTransform } from '@angular/core';
import { Task } from '../../core/models/task.model';

@Pipe({
  name: 'taskFilter',
  standalone: true
})
export class TaskFilterPipe implements PipeTransform {

  transform(
    tasks: Task[],
    searchText: string,
    status: 'all' | 'completed' | 'pending'
  ): Task[] {
    if (!tasks) return [];

    let filtered = tasks;

    // Filter by title
    if (searchText) {
      const lower = searchText.toLowerCase();
      filtered = filtered.filter(t =>
        t.title.toLowerCase().includes(lower)
      );
    }

    // Filter by status
    if (status === 'completed') {
      filtered = filtered.filter(t => t.isCompleted);
    } else if (status === 'pending') {
      filtered = filtered.filter(t => !t.isCompleted);
    }

    return filtered;
  }
}
