import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 

  constructor(public accountService:AccountService,private router:Router,private toastr:ToastrService) {}
  model:any={};
  currentUser$:Observable<User | null>= of(null);
  ngOnInit(): void {
    this.currentUser$=this.accountService.currnetUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next:()=>this.router.navigateByUrl('/members'),
    })
  }

  logOut(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
  


}
