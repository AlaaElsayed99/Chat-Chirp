import { ResolveFn } from '@angular/router';
import { Member } from '../_models/member';
import { inject } from '@angular/core';
import { MemberService } from '../_services/member.service';

export const memberDetailsResolver: ResolveFn<Member> = (route, state) => {
 const memberService= inject(MemberService);
 return memberService.getMember(route.paramMap.get('username')!); 
};
