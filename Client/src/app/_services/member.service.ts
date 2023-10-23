import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginationResult } from '../_models/Pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
 baseUrl=environment.baseUrl;
 members:Member[]=[];
 memberCache=new Map();
 user:User |undefined;
 userParams:UserParams |undefined;
  constructor(private http:HttpClient, private accountService:AccountService) { 
    this.accountService.currnetUser$.pipe(take(1)).subscribe({
      next:user=>{
        if(user){
          this.userParams=new UserParams(user);
          this.user=user;
        }
      }
    })
  }
  getUserParams(){
  return this.userParams;
  }
  setUserParams(params:UserParams){
    this.userParams= params;
  }

  resetUserParams(){
    if(this.user){
      this.userParams=new UserParams(this.user);
      return this.userParams;
    }
    return;
  }
  getMembers(UserParams:UserParams){
    const response= this.memberCache.get(Object.values(UserParams).join('-'));
    if(response) return of(response);
    let params = this.getPaginationHeaders(UserParams.pageNumber,UserParams.pageSize);
    params=params.append('minAge',UserParams.minAge);
    params=params.append('maxAge',UserParams.maxAge);
    params=params.append('gender',UserParams.gender);
    params=params.append('orderBy',UserParams.orderBy);


    return this.getPagintedResult<Member[]>(this.baseUrl+'user',params).pipe(
      map(re=> {
        this.memberCache.set(Object.values(UserParams).join('-'),re);
        return re;
      })
    );;
     
    ;
  }
  
getMember(username:string){
  const member= [...this.memberCache.values()]
  .reduce((arr,ele)=>arr.concat(ele.result),[])
  .find((member:Member)=>member.userName===username);
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
setMainPhoto(photoId:number){
  return this.http.put(this.baseUrl+'user/set-main-photo/'+photoId,{});
}
deletePhoto(photoId:number){
  return this.http.delete(this.baseUrl+'user/delete-photo/'+photoId);
}
private getPagintedResult<T>(url:string,params:HttpParams){
  const paginatedResult:PaginationResult<T>=new PaginationResult<T>
return this.http.get<T>(url, {observe:'response',params}).pipe(
    map(resopne=> {
      if(resopne.body){
        paginatedResult.result=resopne.body;

      }
      const pagination= resopne.headers.get('pagination');
      if(pagination){
        paginatedResult.pagination=JSON.parse(pagination);
      }
      return paginatedResult;
    })
)}

private getPaginationHeaders(pageNumber:number,pageSize:number) {
  let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

  return params;
}

  
}
