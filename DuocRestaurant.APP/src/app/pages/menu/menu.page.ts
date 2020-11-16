import { Component, OnInit } from '@angular/core';
import { Recipe } from '@domain/recipe';
import { Table } from '@domain/table';
import { TableService } from '@services/table.service';
import { AuthService } from '@services/auth.service';
import { User } from '@domain/user';
import { Order } from '@domain/order';
import { LoadingController, ModalController } from '@ionic/angular';
import { ProductService } from '@services/product.service';
import { ProductTypes, OrderState } from '@domain/enums';
import { Product } from '@domain/product';
import { RecipeService } from '@services/recipe.service';
import { OrderDetail } from '@domain/order-detail';
import { CartPage } from './cart/cart.page';
import { OrderService } from '@services/order.service';
import { OrdersPage } from './orders/orders.page';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.page.html',
    styleUrls: ['./menu.page.scss'],
})
export class MenuPage implements OnInit {

    recipes: Recipe[] = [];
    consumables: Product[] = [];
    view: ViewMode = ViewMode.Recipes;
    viewModes = ViewMode;
    table: Table = null;
    order: Order = new Order();
    orders: Order[] = [];
    orderStates = OrderState;

    constructor(private tableService: TableService,
        private orderService: OrderService,
        private productService: ProductService,
        private recipeService: RecipeService,
        private authService: AuthService,
        private loadingController: LoadingController,
        private modalController: ModalController
    ) { }

    ngOnInit() {
        this.checkTableAssociate(null);
    }

    checkTableAssociate = async (event) =>  {
        let loading = await this.loadingController.create({
            message: `Validando mesa`
        });
        await loading.present();

        this.authService.loggedUser.subscribe((user: User) => {
            let tableFilter = {
                UserId: user.Id
            };

            this.tableService.filterBy(tableFilter).subscribe((tables: Table[]) => {
                loading.dismiss();

                if (tables && tables[0]) {
                    this.table = tables[0];

                    this.order.UserId = user.Id;
                    this.order.TableId = this.table.Id;

                    this.getRecipes();
                }
                else {
                    this.table = null;
                }

                if (event)
                    event.target.complete();
            }
            , () => {
                this.table = null;
                loading.dismiss();
                if (event)
                    event.target.complete();
                });

            let orderFilter = {
                UserId: user.Id,
                States: [this.orderStates.Pending, this.orderStates.InPreparation, this.orderStates.Ready],
                PurchaseId: null
            };
            this.orderService.filterBy(orderFilter).subscribe((orders: Order[]) => {
                this.orders = orders;
            })
        });
    };

    viewCart = async () => {
        const modal = await this.modalController.create({
            component: CartPage,
            componentProps: {
                order: this.order
            }
        });

        modal.onWillDismiss().then((value) => {
            if (value.data) {
                if (value.data.Id) {
                    this.order = new Order();
                    this.orders.push(value.data);
                }
            }
        });

        return await modal.present();
    };

    viewOrders = async () => {
        const modal = await this.modalController.create({
            component: OrdersPage,
            componentProps: {
                orders: this.orders
            }
        });

        modal.onWillDismiss().then((value) => {
            console.log('viewOrders dismiss: ', value);
        });

        return await modal.present();
    };

    getRecipes = async () =>  {
        let loading = await this.loadingController.create({
            message: `Buscando platos`
        });
        await loading.present();

        this.recipeService.getAll().subscribe((recipes: Recipe[]) => {
            loading.dismiss();
            this.recipes = recipes;
        }, () => {
            loading.dismiss();
            this.recipes = [];
        });
    };

    getConsumables = async () =>  {
        let loading = await this.loadingController.create({
            message: `Buscando bebestibles`
        });
        await loading.present();

        let filters = {
            ProductTypeId: ProductTypes.Consumable
        };
        this.productService.filterBy(filters).subscribe((products: Product[]) => {
            loading.dismiss();
            this.consumables = products;
        }, () => {
            loading.dismiss();
            this.consumables = [];
        });
    };

    addConsumable = async (consumable: Product) => {
        let addToDetails = () => {
            let orderDetail: OrderDetail = {
                Count: 1,
                Price: consumable.Price,
                ProductId: consumable.Id,
                Product: consumable,
                OrderId: 0
            };
            this.order.OrderDetails.push(orderDetail);
        };

        if (this.order.OrderDetails.length === 0) {
            addToDetails();
        }
        else {
            let found = this.order.OrderDetails.find(detail => detail.ProductId === consumable.Id);
            if (found) {
                found.Count++;
            }
            else {
                addToDetails();
            }
        }
    };

    addRecipe = async (recipe: Recipe) => {
        let addToDetails = () => {
            let orderDetail: OrderDetail = {
                Count: 1,
                Price: recipe.Price,
                OrderId: 0,
                RecipeId: recipe.Id,
                Recipe: recipe
            };
            this.order.OrderDetails.push(orderDetail);
        };

        if (this.order.OrderDetails.length === 0) {
            addToDetails();
        }
        else {
            let found = this.order.OrderDetails.find(detail => detail.RecipeId === recipe.Id);
            if (found) {
                found.Count++;
            }
            else {
                addToDetails();
            }
        }
    };

    changeViewMode = (mode: ViewMode) => {
        if (this.view != mode) {
            this.view = mode;

            if (this.view === this.viewModes.Recipes) {
                this.getRecipes();
            }
            else if (this.view === this.viewModes.Consumables) {
                this.getConsumables();
            }
        }
    };
}

enum ViewMode {
    Recipes,
    Consumables
}