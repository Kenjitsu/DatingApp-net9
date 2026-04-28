import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberCardComponent } from '../member-card/member-card.component';
import { MembersService } from '../../../core/services/members.service';
import { Observable } from 'rxjs';
import { Member } from '../../../types/member';
import { AsyncPipe } from '@angular/common';
import { PaginatedResult } from '../../../types/pagination';
import { Paginator } from "../../../shared/paginator/paginator";

@Component({
    selector: 'app-member-list',
    imports: [MemberCardComponent, AsyncPipe, Paginator],
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  private memberService = inject(MembersService);
  protected paginatedMembers = signal<PaginatedResult<Member>| null>(null);
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadMembers()
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: result => {
        this.paginatedMembers.set(result);
      }
    });
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageNumber;
    this.loadMembers();
  }
}
