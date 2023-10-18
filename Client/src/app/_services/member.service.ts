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
    return this.http.get<Member[]>(this.baseUrl+"user");
  }

getMember(userName:string){
  return this.http.get<Member[]>(this.baseUrl+"user/"+userName,this.getMemberOptions());
}
getMemberOptions(){
    const userString=localStorage.getItem('user');
    if(!userString) return;
    const user=JSON.parse(userString);
    return {
      headers:new HttpHeaders({
        Authorization: 'Bearer '+user.token
      })
    }
  }
}
