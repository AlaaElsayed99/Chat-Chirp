import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
 baseUrl=environment.baseUrl;
 members:Member[]=[];
  constructor(private http:HttpClient) { }
  getMembers(){
    if(this.members.length>0) return of (this.members);
    return this.http.get<Member[]>(this.baseUrl+"user").pipe(
      map(members=>{
        this.members=members;
        return members;
      })
    );
  }

getMember(username:string){
  const member =this.members.find(x=>x.userName==username);
  if(member) return of(member);
  return this.http.get<Member>(this.baseUrl+"user/"+username);
}
updateMember(member:Member){
  return this.http.put(this.baseUrl+"user/",member).pipe(
    map(()=>{
      const index=this.members.indexOf(member);
      this.members[index]={...this.members[index],...member}
    })
  );
}
  
}
