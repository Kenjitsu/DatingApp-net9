import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { of, tap } from 'rxjs';
import { Member } from '../../types/member';
import { Photo } from '../../types/photo';
import { AccountService } from './account.service';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl: string = environment.apiUrl;
  members = signal<Member[]>([]);
  editMode = signal(false);

  
  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'members');
  }

  getMember(id: string) {
    return this.http.get<Member>(this.baseUrl + 'members/' + id);
  }

  getMemberPhotos(id: string) {
    return this.http.get<Photo[]>(this.baseUrl + 'members/' + id + '/photos')
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

  // updateUser(member: Member) {
  //   return this.http.put(this.baseUrl + 'users', member).pipe(
  //     tap(() => {
  //       this.members.update(members => members.map(m => m.username === member.username ? member : m));
  //     })
  //   );
  // }

  // setMainPhoto(photo: Photo) {
  //   return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
  //     tap(() => {
  //       this.members.update(members => members.map(m => {
  //         if (m.photos.includes(photo)) {
  //           m.photoUrl = photo.url;
  //         }
  //         return m;
  //       }))
  //     })
  //   );
  // }

  // deletePhoto(photo: Photo) {
  //   return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
  //     tap(() => {
  //       this.members.update(members => members.map(m => {
  //         if (m.photos.includes(photo)) {
  //           m.photos = m.photos.filter(x => x.id !== photo.id)
  //         }
  //         return m;
  //       }))
  //     })
  //   );
  // }
}
