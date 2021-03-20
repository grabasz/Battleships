import { Injectable } from '@angular/core';
import { BehaviorSubject, from, merge, Observable, Subject, zip } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShipsProviderService {
  asyncShipSize: Observable<[number, void]>;
  nextShipSizeTrigger: Subject<void>;
  constructor() {
    this.setUpShipSizeQueue();
  }

  private setUpShipSizeQueue() {
    this.nextShipSizeTrigger  = new Subject<void>();
    const o = from([5, 4, 4]);
    this.asyncShipSize = zip(o, this.nextShipSizeTrigger);
  }

  reset() {
    this.setUpShipSizeQueue();
  }
}
