import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm:NgForm|undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if(this.editForm?.dirty){
      $event.returnValue=true;
    }
  }
  member:Member|undefined;
  user:User|null=null;
  constructor(private accountService:AccountService,private memberService:MemberService ,private toastr:ToastrService) {
   this.accountService.currnetUser$.pipe(take(1)).subscribe({
    next:user=>this.user=user,
   })
    
  }
  ngOnInit(): void {
   this.loadMember();
  }
loadMember(){
  if(!this.user)return;
  this.memberService.getMember(this.user.username).subscribe({
    next:member=>this.member=member
  })
}
updateMember(){
this.memberService.updateMember(this.editForm?.value).subscribe({
  next:()=>{
  this.toastr.success("updated");
this.editForm?.reset(this.member);
  }
})


}


}
