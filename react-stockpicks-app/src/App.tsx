import './App.css'
import { Footer } from './components/Footer'
import { Header } from './components/Header'
import { AddStockPick } from './pages/AddStockPick'
import { StockPicksList } from './pages/StockPicksList'
import { BrowserRouter, Route, Routes } from 'react-router'

function App() {

  return (
    <BrowserRouter>
      <Header/>
      <Routes>
        <Route path="/" element={<StockPicksList/>}/>
        <Route path="/addstock" element={<AddStockPick/>}/>
      </Routes>
      <Footer/>
    </BrowserRouter>
  )
}

export default App
