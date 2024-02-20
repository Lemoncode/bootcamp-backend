import { Component, OnInit, Input } from '@angular/core';
import { Hero } from '../hero';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { HeroService } from '../hero.service';

@Component({
  selector: 'app-hero-detail',
  templateUrl: './hero-detail.component.html',
  styleUrls: ['./hero-detail.component.css'],
})
export class HeroDetailComponent implements OnInit {
  @Input() hero?: Hero;
  alterEgoPic?: any;

  constructor(
    private route: ActivatedRoute,
    private heroService: HeroService,
    private location: Location,
  ) {}

  ngOnInit(): void {
    this.getHero();
  }

  getHero(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.heroService.getHero(id).subscribe(hero => this.hero = hero);
    this.getAlterEgoPic();
  }

  getAlterEgoPic(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.heroService.getAlterEgoPic(id).subscribe(alterEgoPic => {
      let reader = new FileReader();
      reader.onload = (e: any) => {
        this.alterEgoPic = e.target.result;
      };

      if (alterEgoPic) {
        reader.readAsDataURL(alterEgoPic);
      }
    });
  }

  goBack(): void {
    this.location.back();
  }

  save(): void {
    if (this.hero) {
      this.heroService.updateHero(this.hero).subscribe(() => this.goBack());
    }
  }

  receiveMessage($event: any) {

    if ($event == "newAlterEgoImage") {
      console.log("A new image was uploaded");
      this.getAlterEgoPic();
    }
  }
}
