import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { of, tap } from 'rxjs';
import { EditableMember, Member } from '../../types/member';
import { Photo } from '../../types/photo';
import { AccountService } from './account.service';
import { PaginatedResult } from '../../types/pagination';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl: string = environment.apiUrl;
  member = signal<Member | null>(null);
  editMode = signal(false);

  
  getMembers(pageNumber = 1, pageSize = 5) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    console.log(params);

    return this.http.get<PaginatedResult<Member>>(this.baseUrl + 'members', {params: params});
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id).pipe(
      tap(member => {
        this.member.set(member)
      })
    );
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + '/photos')
  }

  updateMember(member: EditableMember) {
    return this.http.put(this.baseUrl + 'members', member);
  }

  uploadPhoto(file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<Photo>(this.baseUrl + 'members/add-photo', formData);
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'members/set-main-photo/' + photo.id, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'members/delete-photo/' + photoId);
  }

  // getMembers() {
  //   return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
  //     next: (members) => this.members.set(members),
  //   });
  // }

  // getMember(username: string) {
  //   const member = this.members().find(x => x.username === username);
  //   if (member !== undefined) return of(member);
    
  //   return this.http.get<Member>(this.baseUrl + 'users/' + username);
  // }

  

  
}

