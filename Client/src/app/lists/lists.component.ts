import { Component, OnInit } from '@angular/core';
import { MemberService } from '../_services/member.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit{
  /**
   *
   */
  constructor(private memberSrvice:MemberService) {
    
  }
  ngOnInit(): void {
   
  }
  getMembers(){
   return this.memberSrvice.getMembers().subscribe({
    next:response=>console.log(response),
    error:err=>console.log(err),
    complete:()=>console.log("has completed")
  })
  }

}
