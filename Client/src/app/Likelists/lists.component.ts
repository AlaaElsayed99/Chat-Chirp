import { Component, OnInit } from '@angular/core';
import { MemberService } from '../_services/member.service';
import { Member } from '../_models/member';
import { TimeagoModule } from 'ngx-timeago';
import { Pagination } from '../_models/Pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
  
})
export class ListsComponent implements OnInit{
  members:Member[]|undefined;
  predicate= 'liked';
  pageNumber=1;
  pageSize=5;

  pagination:Pagination|undefined;
  constructor(private memberService:MemberService) {
    
  }
  ngOnInit(): void {
  this.loadlikes();
  }
  loadlikes(){
    this.memberService.getLikes(this.predicate,this.pageNumber,this.pageSize).subscribe({
      next:response=>{
        this.members=response.result;
        this.pagination=response.pagination;

      }
    })
  }
  pageChanged(event: any){
    if(this.pageNumber!==event.page){
      this.pageNumber=event.page;
      this.loadlikes();
  }
}
}


