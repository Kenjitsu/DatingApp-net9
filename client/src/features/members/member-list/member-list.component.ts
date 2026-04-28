import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MemberCardComponent } from '../member-card/member-card.component';
import { MembersService } from '../../../core/services/members.service';
import { Member, MemberParams } from '../../../types/member';
import { PaginatedResult } from '../../../types/pagination';
import { Paginator } from "../../../shared/paginator/paginator";
import { FilterModal } from '../filter-modal/filter-modal';

@Component({
    selector: 'app-member-list',
    imports: [MemberCardComponent, Paginator, FilterModal],
    templateUrl: './member-list.component.html',
    styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  @ViewChild('filterModal') modal!: FilterModal
  private memberService = inject(MembersService);
  protected paginatedMembers = signal<PaginatedResult<Member>| null>(null);
  protected memberParams = new MemberParams();

  ngOnInit(): void {
    this.loadMembers()
  }

  loadMembers() {
    this.memberService.getMembers(this.memberParams).subscribe({
      next: result => {
        this.paginatedMembers.set(result);
      }
    });
  }

  onPageChange(event: { pageNumber: number, pageSize: number }) {
    this.memberParams.pageSize = event.pageSize;
    this.memberParams.pageNumber = event.pageNumber;
    this.loadMembers();
  }

  openModal() {
    this.modal.open();
  }

  onClose() {
    console.log('Modal close');
  }

  onFilterChange(data: MemberParams) {
    this.memberParams = data;
    this.loadMembers();
  }

  resetFilters() {
    this.memberParams = new MemberParams();
    this.loadMembers();
  }
}
