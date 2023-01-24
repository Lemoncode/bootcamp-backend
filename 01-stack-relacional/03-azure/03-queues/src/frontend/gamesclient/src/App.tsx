import { BrowserRouter } from 'react-router-dom';
import './App.css';
import { DefaultRouter } from './routes/routes';

function App() {
  return (
    <BrowserRouter>
      <DefaultRouter />
      <div className="App">
      </div>
    </BrowserRouter>
  );
}

export default App;
