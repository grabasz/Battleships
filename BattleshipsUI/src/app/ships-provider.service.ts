import { Injectable } from '@angular/core';
import { BehaviorSubject, from, merge, Observable, Subject, zip } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShipsProviderService {
  asyncShipSize: Observable<[number, void]>;
  listener: Subject<void> = new Subject<void>();
  constructor() {
    const o =from([5,4,4]);
    this.asyncShipSize = zip(o, this.listener);
  }
}
