import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Pagination } from 'src/app/_models/Pagination';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // members$:Observable<Member[]>|undefined;
  members:Member[]=[];
  pagination: Pagination |undefined ;
  userParams:UserParams |undefined ;
  user:User|undefined;
  genderList=[{value:'male',display:'Females'},{value:'female',display:'Males'}]
  constructor(private memberService:MemberService) {
   this.userParams=this.memberService.getUserParams();
  }
  ngOnInit(): void {
  //  this.members$=this.memberService.getMembers();
  this.loadMembers();
  }
  loadMembers(){
    if(this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next:resopnse=>
        {
          if(resopnse.result&&resopnse.pagination){
            this.members=resopnse.result;
            this.pagination=resopnse.pagination;
          }
        }
  
      })
    }
    
  }
  resetFilters(){
    
      this.userParams=this.memberService.resetUserParams();
      this.loadMembers();
    
  }
  pageChanged(event: any){
    if(this.userParams&& this.userParams?.pageNumber!==event.page){
      this.userParams.pageNumber=event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
  }
  
}


}
